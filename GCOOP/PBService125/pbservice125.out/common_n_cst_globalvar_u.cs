//**************************************************************************
//
//                        Sybase Inc. 
//
//    THIS IS A TEMPORARY FILE GENERATED BY POWERBUILDER. IF YOU MODIFY 
//    THIS FILE, YOU DO SO AT YOUR OWN RISK. SYBASE DOES NOT PROVIDE 
//    SUPPORT FOR .NET ASSEMBLIES BUILT WITH USER-MODIFIED CS FILES. 
//
//**************************************************************************

#line 1 "c:\\gcoop_all\\core\\gcoop\\pbservice125\\pbsrvcom\\common.pbl\\common.pblx\\n_cst_globalvar.sru"
#line hidden
#line 1 "n_cst_globalvar"
#line hidden
[Sybase.PowerBuilder.PBGroupAttribute("n_cst_globalvar",Sybase.PowerBuilder.PBGroupType.UserObject,"","c:\\gcoop_all\\core\\gcoop\\pbservice125\\pbsrvcom\\common.pbl\\common.pblx",null,"pbservice125")]
internal class c__n_cst_globalvar : c__n_cst_base
{
	#line hidden

	#line 1 "n_cst_globalvar.of_getitemstring(SS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_getitemstring", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBString of_getitemstring(Sybase.PowerBuilder.PBString as_variable)
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_value = Sybase.PowerBuilder.PBString.DefaultValue;

		Sybase.PowerBuilder.IPBValue[] __PB_TEMP_DB__OutputVars0 = new Sybase.PowerBuilder.IPBValue[1];
		__PB_TEMP_DB__OutputVars0[0] = ls_value;
		Sybase.PowerBuilder.DSI.PBSQL.DSISQLFunc.Select(Sybase.PowerBuilder.WPF.PBSession.CurrentSession,
			new Sybase.PowerBuilder.DB.DBStatement(
				new Sybase.PowerBuilder.DB.DBStatement(
					new System.String[2] {
						"select var_value     from webvariable    where var_name = ",
						"    "
					},
					new Sybase.PowerBuilder.IPBValue[1] {
						as_variable
					}
				),
				new Sybase.PowerBuilder.DB.DBOutputVarInfo[1] {
					new Sybase.PowerBuilder.DB.DBOutputVarInfo(false)
				}
			),
			new Sybase.PowerBuilder.IPBValue[1] {
				as_variable
			},
			__PB_TEMP_DB__OutputVars0,
			new Sybase.PowerBuilder.PBDataType[1] {
				Sybase.PowerBuilder.PBDataType.String
			},
			itr_transaction
		);

		#line 19
		ls_value = (Sybase.PowerBuilder.PBString) __PB_TEMP_DB__OutputVars0[0];
		#line hidden
		#line 24
		if ((Sybase.PowerBuilder.PBBoolean)(itr_transaction.SQLCode != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
		#line hidden
		{
			#line 25
			this.of_seterror(itr_transaction.SQLErrText);
			#line hidden
			#line 26
			ls_value.SetNull();
			#line hidden
		}
		#line 28
		return ls_value;
		#line hidden
	}

	#line 1 "n_cst_globalvar.of_getitemnumber(DS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_getitemnumber", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBDouble of_getitemnumber(Sybase.PowerBuilder.PBString as_variable)
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_value = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBDouble ldbl_value = Sybase.PowerBuilder.PBDouble.DefaultValue;
		Sybase.PowerBuilder.PBException th = null;
		try
		{
			try
			{

				Sybase.PowerBuilder.IPBValue[] __PB_TEMP_DB__OutputVars0 = new Sybase.PowerBuilder.IPBValue[1];
				__PB_TEMP_DB__OutputVars0[0] = ls_value;
				Sybase.PowerBuilder.DSI.PBSQL.DSISQLFunc.Select(Sybase.PowerBuilder.WPF.PBSession.CurrentSession,
					new Sybase.PowerBuilder.DB.DBStatement(
						new Sybase.PowerBuilder.DB.DBStatement(
							new System.String[2] {
								"select var_value     from webvariable    where var_name = ",
								"    "
							},
							new Sybase.PowerBuilder.IPBValue[1] {
								as_variable
							}
						),
						new Sybase.PowerBuilder.DB.DBOutputVarInfo[1] {
							new Sybase.PowerBuilder.DB.DBOutputVarInfo(false)
						}
					),
					new Sybase.PowerBuilder.IPBValue[1] {
						as_variable
					},
					__PB_TEMP_DB__OutputVars0,
					new Sybase.PowerBuilder.PBDataType[1] {
						Sybase.PowerBuilder.PBDataType.String
					},
					itr_transaction
				);

				#line 21
				ls_value = (Sybase.PowerBuilder.PBString) __PB_TEMP_DB__OutputVars0[0];
				#line hidden
				#line 26
				if ((Sybase.PowerBuilder.PBBoolean)(itr_transaction.SQLCode != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
				#line hidden
				{
					#line 27
					this.of_seterror(itr_transaction.SQLErrText);
					#line hidden
					#line 28
					ldbl_value.SetNull();
					#line hidden
				}
				#line 30
				ldbl_value = Sybase.PowerBuilder.WPF.PBSystemFunctions.Double((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_value))));
				#line hidden
				#line 31
				return ldbl_value;
				#line hidden
			}
			catch (System.DivideByZeroException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0001);
				throw new System.Exception();
			}
			catch (System.NullReferenceException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0002);
				throw new System.Exception();
			}
			catch (System.IndexOutOfRangeException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0003);
				throw new System.Exception();
			}
		}
		#line 32
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_th__temp)
		#line hidden
		{
			th = __PB_TEMP_th__temp.E;
			#line 33
			ldbl_value.SetNull();
			#line hidden
			#line 34
			return ldbl_value;
			#line hidden
		}
	}

	#line 1 "n_cst_globalvar.of_getitemnumber_relative(DSD)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_getitemnumber_relative", new string[]{"string", "double"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBDouble of_getitemnumber_relative(Sybase.PowerBuilder.PBString as_variable, Sybase.PowerBuilder.PBDouble adbl_relative)
	{
		#line hidden
		Sybase.PowerBuilder.PBString ls_value = Sybase.PowerBuilder.PBString.DefaultValue;
		Sybase.PowerBuilder.PBDouble ldbl_value = Sybase.PowerBuilder.PBDouble.DefaultValue;
		Sybase.PowerBuilder.PBException th = null;
		try
		{
			try
			{

				Sybase.PowerBuilder.IPBValue[] __PB_TEMP_DB__OutputVars0 = new Sybase.PowerBuilder.IPBValue[1];
				__PB_TEMP_DB__OutputVars0[0] = ls_value;
				Sybase.PowerBuilder.DSI.PBSQL.DSISQLFunc.Select(Sybase.PowerBuilder.WPF.PBSession.CurrentSession,
					new Sybase.PowerBuilder.DB.DBStatement(
						new Sybase.PowerBuilder.DB.DBStatement(
							new System.String[2] {
								"select var_value     from webvariable    where var_name = ",
								"    "
							},
							new Sybase.PowerBuilder.IPBValue[1] {
								as_variable
							}
						),
						new Sybase.PowerBuilder.DB.DBOutputVarInfo[1] {
							new Sybase.PowerBuilder.DB.DBOutputVarInfo(false)
						}
					),
					new Sybase.PowerBuilder.IPBValue[1] {
						as_variable
					},
					__PB_TEMP_DB__OutputVars0,
					new Sybase.PowerBuilder.PBDataType[1] {
						Sybase.PowerBuilder.PBDataType.String
					},
					itr_transaction
				);

				#line 21
				ls_value = (Sybase.PowerBuilder.PBString) __PB_TEMP_DB__OutputVars0[0];
				#line hidden
				#line 26
				if ((Sybase.PowerBuilder.PBBoolean)(itr_transaction.SQLCode != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
				#line hidden
				{
					#line 27
					this.of_seterror(itr_transaction.SQLErrText);
					#line hidden
					#line 28
					ldbl_value.SetNull();
					#line hidden
					#line 29
					return ldbl_value;
					#line hidden
				}
				#line 31
				ldbl_value = Sybase.PowerBuilder.WPF.PBSystemFunctions.Double((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ls_value))))+ adbl_relative;
				#line hidden
				#line 32
				ls_value = Sybase.PowerBuilder.WPF.PBSystemFunctions.String((Sybase.PowerBuilder.PBAny)(((Sybase.PowerBuilder.PBAny)(ldbl_value))));
				#line hidden

				#line 33
				Sybase.PowerBuilder.DSI.PBSQL.DSISQLFunc.Update(Sybase.PowerBuilder.WPF.PBSession.CurrentSession,
					new Sybase.PowerBuilder.DB.DBStatement(
						new System.String[3] {
							"update webvariable    set var_value = ",
							"    where var_name = ",
							"    "
						},
						new Sybase.PowerBuilder.IPBValue[2] {
							ls_value,
							as_variable
						}
					),
					new Sybase.PowerBuilder.IPBValue[2] {
						ls_value,
						as_variable
					},
					itr_transaction
				);
				#line hidden
				#line 37
				if ((Sybase.PowerBuilder.PBBoolean)(itr_transaction.SQLCode != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
				#line hidden
				{
					#line 38
					this.of_seterror(itr_transaction.SQLErrText);
					#line hidden
					#line 39
					ldbl_value.SetNull();
					#line hidden
				}
				#line 41
				return ldbl_value;
				#line hidden
			}
			catch (System.DivideByZeroException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0001);
				throw new System.Exception();
			}
			catch (System.NullReferenceException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0002);
				throw new System.Exception();
			}
			catch (System.IndexOutOfRangeException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0003);
				throw new System.Exception();
			}
		}
		#line 42
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_th__temp)
		#line hidden
		{
			th = __PB_TEMP_th__temp.E;
			#line 43
			ldbl_value.SetNull();
			#line hidden
			#line 44
			return ldbl_value;
			#line hidden
		}
	}

	#line 1 "n_cst_globalvar.of_getitem(SS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_getitem", new string[]{"string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBString of_getitem(Sybase.PowerBuilder.PBString as_variable)
	{
		#line hidden
		#line 18
		return this.of_getitemstring(as_variable);
		#line hidden
	}

	#line 1 "n_cst_globalvar.of_setitem(ISS)"
	#line hidden
	[Sybase.PowerBuilder.PBFunctionAttribute("of_setitem", new string[]{"string", "string"}, Sybase.PowerBuilder.PBModifier.Public, Sybase.PowerBuilder.PBFunctionType.kPowerscriptFunction)]
	public virtual Sybase.PowerBuilder.PBInt of_setitem(Sybase.PowerBuilder.PBString as_variable, Sybase.PowerBuilder.PBString as_value)
	{
		#line hidden
		Sybase.PowerBuilder.PBException th = null;
		try
		{
			try
			{

				#line 18
				Sybase.PowerBuilder.DSI.PBSQL.DSISQLFunc.Update(Sybase.PowerBuilder.WPF.PBSession.CurrentSession,
					new Sybase.PowerBuilder.DB.DBStatement(
						new System.String[3] {
							"update webvariable    set var_value = ",
							"    where var_name = ",
							"    "
						},
						new Sybase.PowerBuilder.IPBValue[2] {
							as_value,
							as_variable
						}
					),
					new Sybase.PowerBuilder.IPBValue[2] {
						as_value,
						as_variable
					},
					itr_transaction
				);
				#line hidden
				#line 22
				if ((Sybase.PowerBuilder.PBBoolean)(itr_transaction.SQLCode != (Sybase.PowerBuilder.PBLong)(new Sybase.PowerBuilder.PBInt(0))))
				#line hidden
				{
					#line 23
					this.of_seterror(itr_transaction.SQLErrText);
					#line hidden
					#line 24
					return new Sybase.PowerBuilder.PBInt(-1);
					#line hidden
				}
				#line 26
				return new Sybase.PowerBuilder.PBInt(1);
				#line hidden
			}
			catch (System.DivideByZeroException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0001);
				throw new System.Exception();
			}
			catch (System.NullReferenceException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0002);
				throw new System.Exception();
			}
			catch (System.IndexOutOfRangeException)
			{
				Sybase.PowerBuilder.PBRuntimeError.Throw(Sybase.PowerBuilder.RuntimeErrorCode.RT_R0003);
				throw new System.Exception();
			}
		}
		#line 27
		catch (Sybase.PowerBuilder.PBExceptionE __PB_TEMP_th__temp)
		#line hidden
		{
			th = __PB_TEMP_th__temp.E;
			#line 28
			this.of_seterror(th.GetMessage());
			#line hidden
			#line 29
			return new Sybase.PowerBuilder.PBInt(-1);
			#line hidden
		}
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("create")]
	public override void create()
	{
		#line hidden
		#line hidden
		base.create();
		#line hidden
	}

	#line hidden
	[Sybase.PowerBuilder.PBEventAttribute("destroy")]
	public override void destroy()
	{
		#line hidden
		#line hidden
		base.destroy();
		#line hidden
	}

	void _init()
	{
		this.CreateEvent += new Sybase.PowerBuilder.PBEventHandler(this.create);
		this.DestroyEvent += new Sybase.PowerBuilder.PBEventHandler(this.destroy);

		if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
		{
		}
	}

	public c__n_cst_globalvar()
	{
		_init();
	}


	public static explicit operator c__n_cst_globalvar(Sybase.PowerBuilder.PBAny v)
	{
		if (v.Value is Sybase.PowerBuilder.PBUnboundedAnyArray)
		{
			Sybase.PowerBuilder.PBUnboundedAnyArray a = (Sybase.PowerBuilder.PBUnboundedAnyArray)v.Value;
			c__n_cst_globalvar vv = new c__n_cst_globalvar();
			if (vv.FromUnboundedAnyArray(a) > 0)
			{
				return vv;
			}
			else
			{
				return null;
			}
		}
		else
		{
			return (c__n_cst_globalvar) v.Value;
		}
	}
}
 