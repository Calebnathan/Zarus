# Cline Automation for Zarus Unity Project

This directory contains Cline hooks and workflows specifically designed for the Zarus pandemic simulation game development. These automation tools leverage the Unity MCP server and GitHub MCP server integrations to streamline common Unity development tasks.

## 🎯 Overview

The automation system includes:
- **Hooks**: Automated quality checks triggered by development events
- **Workflows**: Complex multi-step automation for common tasks
- **Custom Actions**: Specialized functions for Unity game development

## 🔗 Prerequisites

Ensure you have the following MCP servers configured in Cline:
- **Unity MCP Server**: For Unity Editor integration (`unity`)
- **GitHub MCP Server**: For repository management (`github.com/github/github-mcp-server`)

## 📋 Available Hooks

### Pre-Commit Hook
Automatically runs before each commit to ensure code quality:
- ✅ Unity Console error/warning check
- ✅ C# script validation
- ✅ Edit Mode test execution
- ✅ Assembly definition validation

**Usage**: Automatically triggered on `git commit`

### Post-Merge Hook
Runs after merging branches to maintain project state:
- 🔄 Unity asset database refresh
- 🗺️ Region asset rebuilding (if GIS data changed)
- 📊 Project state validation

**Usage**: Automatically triggered on `git merge`

### Pre-Build Hook
Validates project before building:
- ⚙️ Build settings validation
- 🧪 Play Mode test execution
- 📦 Assembly reference validation

**Usage**: Triggered before Unity builds

### On-File-Save Hook
Real-time validation when files are saved:
- **C# Scripts** (`*.cs`): Auto-validation
- **UXML Files** (`*.uxml`): Syntax checking
- **USS Files** (`*.uss`): Style validation

**Usage**: Automatically triggered on file save

### Daily Maintenance
Scheduled maintenance tasks:
- 🧹 Cleanup temporary files
- 📦 Dependency updates
- 💾 Project settings backup

**Usage**: Runs daily automatically

## 🚀 Available Workflows

### 1. Setup New Feature
Creates a new feature branch with Unity components.

```bash
cline workflow setup-new-feature --feature_name "infection_visualization" --feature_type "ui"
```

**Parameters**:
- `feature_name` (required): Name of the feature
- `feature_type`: `ui`, `system`, `map`, or `general`

**What it does**:
- Creates GitHub feature branch
- Generates Unity scripts/scenes based on type
- Sets up UI layouts and controllers (for UI features)
- Configures proper namespaces and assembly references

### 2. Deploy to GitHub
Comprehensive deployment with quality checks.

```bash
cline workflow deploy-to-github --pr_title "Add infection visualization system" --pr_description "Implements real-time infection spread visualization"
```

**Parameters**:
- `pr_title` (required): Pull request title
- `pr_description` (optional): Pull request description

**What it does**:
- Runs pre-deployment tests
- Validates all C# scripts
- Checks Unity Console for errors
- Creates GitHub pull request

### 3. Fix Unity Errors
Automated error detection and fixing.

```bash
cline workflow fix-unity-errors
```

**What it does**:
- Reads Unity Console errors
- Analyzes error patterns
- Applies common fixes
- Validates fix success

### 4. Update Province Data
Manages GIS data and region asset regeneration.

```bash
cline workflow update-province-data --source_file "Assets/Sprites/za.json"
```

**Parameters**:
- `source_file` (optional): Path to GIS data file

**What it does**:
- Backs up current region data
- Validates GeoJSON structure
- Rebuilds region assets via Unity menu
- Tests map loading functionality

### 5. Optimize UI Performance
Analyzes and improves UI Toolkit performance.

```bash
cline workflow optimize-ui-performance
```

**What it does**:
- Analyzes UI hierarchy complexity
- Checks UXML/USS performance patterns
- Identifies inefficient selectors
- Generates performance report

### 6. Setup Simulation Test
Configures outbreak simulation testing scenarios.

```bash
cline workflow setup-simulation-test --test_scenario "pandemic" --duration_days 200
```

**Parameters**:
- `test_scenario`: `fast_spread`, `slow_spread`, `controlled_outbreak`, or `pandemic`
- `duration_days` (optional): Simulation length in game days

**What it does**:
- Loads test scene
- Configures simulation parameters
- Starts Unity Play Mode
- Monitors simulation metrics

### 7. Code Review Preparation
Prepares code for review with quality checks.

```bash
cline workflow code-review-prep
```

**What it does**:
- Formats C# code
- Runs static analysis
- Checks Unity naming conventions
- Validates serializable fields
- Generates review checklist

## 🛠️ Custom Actions

The workflows use specialized custom actions:

### Unity Error Analysis
- **Purpose**: Intelligent error pattern recognition
- **Features**: Common fix suggestions, reference validation
- **Usage**: Integrated into `fix-unity-errors` workflow

### GeoJSON Validation
- **Purpose**: Validates map data structure
- **Features**: Schema validation, required field checks
- **Usage**: Integrated into `update-province-data` workflow

### UI Performance Analysis
- **Purpose**: UXML/USS optimization recommendations
- **Features**: Selector efficiency, nesting analysis
- **Usage**: Integrated into `optimize-ui-performance` workflow

### Outbreak Simulation Monitoring
- **Purpose**: Tracks simulation metrics
- **Features**: Real-time data collection, performance analysis
- **Usage**: Integrated into `setup-simulation-test` workflow

## 🎮 Unity-Specific Features

### Assembly Definition Management
- Automatic validation of assembly references
- Detection of circular dependencies
- Build-time optimization checks

### Scene Management
- Automated scene loading for tests
- Scene hierarchy optimization
- Build settings validation

### Asset Pipeline Integration
- GIS data processing automation
- Region asset regeneration
- Material and texture optimization

### UI Toolkit Integration
- UXML syntax validation
- USS performance optimization
- Runtime UI hierarchy analysis

## 📊 Quality Assurance

### Test Integration
- **Edit Mode Tests**: Fast validation during development
- **Play Mode Tests**: Full runtime testing
- **Performance Tests**: Frame rate and memory monitoring

### Code Quality
- **Static Analysis**: Pattern detection and best practices
- **Naming Conventions**: Unity-specific naming validation
- **Serialization Checks**: MonoBehaviour field validation

### Error Prevention
- **Console Monitoring**: Real-time error detection
- **Asset Validation**: Missing reference detection
- **Build Validation**: Pre-build compatibility checks

## 🔧 Configuration

### Environment Variables
```json
{
  "UNITY_PROJECT_PATH": "/home/abusive/Coding/radicazz/Zarus",
  "UNITY_LOG_LEVEL": "INFO",
  "CLINE_WORKFLOW_TIMEOUT": "300"
}
```

### MCP Server Requirements
Ensure these MCP servers are active:
- Unity MCP Server (streamable HTTP on localhost:8080)
- GitHub MCP Server (Docker-based with PAT authentication)

## 🚨 Troubleshooting

### Common Issues

1. **Unity MCP Server Not Connected**
   - Ensure Unity Editor is running
   - Check MCP server endpoint: `http://localhost:8080/mcp`
   - Verify Unity MCP integration is enabled

2. **GitHub Operations Failing**
   - Verify GitHub PAT has required permissions
   - Check repository access rights
   - Confirm Docker is running for GitHub MCP server

3. **Asset Rebuilding Fails**
   - Ensure GeoJSON data is valid
   - Check Unity console for import errors
   - Verify map rebuild menu item exists

4. **Test Execution Timeouts**
   - Increase timeout values in workflow parameters
   - Check Unity project compilation status
   - Verify test assemblies are properly configured

### Debug Commands

```bash
# Check Unity state
cline use-mcp-tool unity manage_editor --action get_state

# Validate GitHub connection
cline use-mcp-tool github.com/github/github-mcp-server get_me

# Read Unity console
cline use-mcp-tool unity read_console --action get --types error,warning
```

## 🎯 Best Practices

1. **Run hooks before major operations**: Ensure quality before committing
2. **Use workflows for repetitive tasks**: Automate common development patterns
3. **Monitor Unity Console**: Keep error count at zero
4. **Test early and often**: Use simulation workflows for validation
5. **Optimize regularly**: Run performance workflows periodically

## 📚 Further Reading

- [Unity MCP Server Documentation](https://docs.unity.com/mcp)
- [GitHub MCP Server Repository](https://github.com/github/github-mcp-server)
- [Cline Documentation](https://docs.cline.bot)
- [Unity 6000.2.10f1 Documentation](https://docs.unity3d.com/6000.2/Documentation/Manual/)
